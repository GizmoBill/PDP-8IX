## You are using the Python ARM Radar Toolkit (Py-ART), an open source
## library for working with weather radar data. Py-ART is partly supported
## by the U.S. Department of Energy Office of Science as part of
## the Atmospheric Radiation Measurement (ARM) User Facility.
##
## If you use this software to prepare a publication, please cite:
##
##     JJ Helmus and SM Collis, JORS 2016, doi: 10.5334/jors.119

# The following code was wrtten by Anthropic Claude, with minor human mods.
# Claude: "If I wrote general-purpose Python accessing an open source library,
# you're in the clear. I'm not asserting any claim to your modifications or the
# combined work"

import sys
import os

# Runtime hook must execute FIRST before ANY pyart imports
# The hook file (runtime_hook_nexrad.py) runs automatically before this script,
# but we need to ensure nexradaws (which imports pyart) comes after hook setup.

import xml.etree.ElementTree as ET
from xml.dom import minidom
import argparse
from datetime import datetime
import logging
from pathlib import Path

# NOW import nexradaws (which will trigger pyart import with hook already in place)
import nexradaws

# NOW import pyart explicitly
import pyart

# *******************
# *                 *
# *  Setup logging  *
# *                 *
# *******************

log_dir = Path("./logs")
log_dir.mkdir(exist_ok=True)
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[
        logging.FileHandler(log_dir / "NexradParser.log"),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)

# *********************************
# *                               *
# *  Runlength Encode dbZ Values  *
# *                               *
# *********************************

def encode_dbz_rle(dbz_values):
    """
    Run-length encode dBZ values with None compression.
    Consecutive None values are encoded as 'zN' where N is the count.
    Valid dBZ values are kept as comma-separated floats.
    
    Example: [1.5, None, None, None, 2.3, None] -> "1.5,z3,2.3,z1"
    """
    encoded = []
    none_count = 0
    
    for val in dbz_values:
        if val is None or (isinstance(val, float) and val != val):  # NaN check
            none_count += 1
        else:
            # Flush any accumulated Nones
            if none_count > 0:
                encoded.append(f"z{none_count}")
                none_count = 0
            # Append the valid value
            encoded.append(str(float(val)))
    
    # Don't forget trailing Nones
    if none_count > 0:
        encoded.append(f"z{none_count}")
    
    return ",".join(encoded)

# ******************************
# *                            *
# *  Fetch Latest Nexrad Data  *
# *                            *
# ******************************

def fetch_latest_nexrad(radar_id, output_dir="./radar_data", verbose=False):
    """
    Fetch latest NEXRAD Level 2 data and export in XML format
    
    Args:
        radar_id: e.g., 'KABR', 'KORD', 'KLOT'
        output_dir: Directory to save data files
    """
    
    output_dir = Path(output_dir)
    output_dir.mkdir(exist_ok=True)
    
    local_filepath = None
    
    try:
        if verbose:
            logger.info(f"Starting fetch for {radar_id}")
        
        # Connect to AWS
        conn = nexradaws.NexradAwsInterface()
        
        # Get today's date
        today = datetime.utcnow()
        year, month, day = today.year, today.month, today.day
        
        if verbose:
            logger.info(f"Querying AWS for {radar_id} on {year}-{month:02d}-{day:02d}")
        results = conn.get_avail_scans(year, month, day, radar_id)
        
        if not results:
            logger.error(f"No data found for {radar_id}")
            return False
        
        # Get latest non-MDM file
        latest_scan = None
        for scan in reversed(results):
            if "MDM" not in str(scan):
                latest_scan = scan
                break
        
        if not latest_scan:
            logger.error("No valid scan files found")
            return False
        
        if verbose:
            logger.info(f"Found: {latest_scan}")
        
        # Download
        if verbose:
            logger.info("Downloading from AWS...")
        download_result = conn.download(latest_scan, ".")
        
        if download_result.failed_count > 0:
            logger.error("Download failed")
            return False
        
        local_file_obj = list(download_result.iter_success())[0]
        local_filepath = local_file_obj.filepath
        source_filename = os.path.basename(local_filepath)

        if verbose:
            logger.info(f"Downloaded: {local_filepath}")
        
        # Get output XML file name
        scan_time = local_file_obj.scan_time
        timestamp_str = scan_time.strftime("%Y%m%d_%H%M%S") if scan_time else datetime.utcnow().strftime("%Y%m%d_%H%M%S")
        output_file = output_dir / f"{radar_id}_{timestamp_str}.xml"

        # If output file already exists, it's the same dataset so just keep it
        if os.path.exists(output_file):
            logger.info(f"{output_file} already exists")
            print(f"[AE] {output_file}")
            return True
        
        # Parse with PyART
        if verbose:
            logger.info("Parsing with PyART...")
        try:
            radar = local_file_obj.open_pyart()
        except Exception as e:
            logger.error(f"Failed to parse radar file: {e}")
            return False
        
        # Extract reflectivity field
        if 'reflectivity' not in radar.fields:
            logger.error("Reflectivity field not found in radar data")
            return False
        
        reflectivity = radar.fields['reflectivity']['data']
        
        # Get bin resolution (gate spacing in meters). Assumes all bins are same.
        start_range = radar.range['data'][0]
        range_res = radar.range['data'][1] - start_range
        
        # Build output structure as XML
        root = ET.Element("nexrad_data")
        
        # Header
        header = ET.SubElement(root, "header")
        ET.SubElement(header, "radar_id").text = radar_id
        ET.SubElement(header, "timestamp").text = scan_time.isoformat() if scan_time else datetime.utcnow().isoformat()
        ET.SubElement(header, "source_filename").text = source_filename
        ET.SubElement(header, "latitude").text = str(float(radar.latitude["data"][0]))
        ET.SubElement(header, "longitude").text = str(float(radar.longitude["data"][0]))
        ET.SubElement(header, "altitude_m").text = str(float(radar.altitude["data"][0]))
        ET.SubElement(header, "start_range_m").text = str(float(start_range))
        ET.SubElement(header, "bin_resolution_m").text = str(float(range_res))
        ET.SubElement(header, "number_of_gates").text = str(int(radar.ngates))
        ET.SubElement(header, "number_of_rays").text = str(int(radar.nrays))
        ET.SubElement(header, "number_of_sweeps").text = str(int(radar.nsweeps))
        
        # Sweeps metadata
        if verbose:
            logger.info("Extracting sweep metadata...")
        sweeps_elem = ET.SubElement(root, "sweeps")
        for sweep_idx in range(radar.nsweeps):
            sweep_slice = radar.get_slice(sweep_idx)
            sweep_start_ray = sweep_slice.start
            sweep_end_ray = sweep_slice.stop - 1  # stop is exclusive, so subtract 1
            num_rays_in_sweep = sweep_slice.stop - sweep_slice.start
            elevation_angle = float(radar.elevation["data"][sweep_start_ray])
            
            sweep = ET.SubElement(sweeps_elem, "sweep")
            ET.SubElement(sweep, "index").text = str(int(sweep_idx))
            ET.SubElement(sweep, "elevation_angle").text = str(elevation_angle)
            ET.SubElement(sweep, "start_ray_index").text = str(int(sweep_start_ray))
            ET.SubElement(sweep, "end_ray_index").text = str(int(sweep_end_ray))
            ET.SubElement(sweep, "num_rays").text = str(int(num_rays_in_sweep))
        
        # Rays
        rays_elem = ET.SubElement(root, "rays")
        if verbose:
            logger.info("Extracting ray data...")
        for ray_idx in range(radar.nrays):
            ray_elem = ET.SubElement(rays_elem, "ray")
            ET.SubElement(ray_elem, "ray_index").text = str(int(ray_idx))
            ET.SubElement(ray_elem, "azimuth").text = str(float(radar.azimuth["data"][ray_idx]))
            ET.SubElement(ray_elem, "elevation").text = str(float(radar.elevation["data"][ray_idx]))
            
            # DBZ values
            dbz_elem = ET.SubElement(ray_elem, "dbz_values")
            dbz_str = encode_dbz_rle(reflectivity[ray_idx].tolist())
            dbz_elem.text = dbz_str
        
        # Save to XML with pretty printing
        xml_str = minidom.parseString(ET.tostring(root)).toprettyxml(indent="  ")

        # Remove extra blank lines
        xml_str = '\n'.join([line for line in xml_str.split('\n') if line.strip()])
        
        with open(output_file, 'w') as f:
            f.write(xml_str)
        
        if verbose:
            logger.info(f"Data written to {output_file}")

        print(f"[OK] {output_file}")
        
        return True
        
    except Exception as e:
        logger.error(f"Error: {e}", exc_info=True)
        return False
    
    finally:
        # Clean up downloaded file
        if local_filepath and os.path.exists(local_filepath):
            try:
                os.remove(local_filepath)
                if verbose:
                    logger.info(f"[OK] Cleaned up: {local_filepath}")
            except Exception as e:
                logger.warning(f"Could not clean up {local_filepath}: {e}")

# ******************
# *                *
# *  Main Program  *
# *                *
# ******************

def main():
    parser = argparse.ArgumentParser(
        description='Fetch latest NEXRAD Level 2 data and export as XML'
    )
    parser.add_argument(
        '--radar',
        required=True,
        help='Radar ID (e.g., KABR, KORD, KLOT)'
    )
    parser.add_argument(
        '--output-dir',
        default='./radar_data',
        help='Output directory for data files (default: ./radar_data)'
    )
    parser.add_argument(
        '--verbose',
        action='store_true',
        help='Generate INFO messages in log file and stdout'
    )
    
    args = parser.parse_args()
    
    if args.verbose:
        logger.info(f"NexradFetcher started for {args.radar}")
    
    # Fetch data
    success = fetch_latest_nexrad(args.radar, args.output_dir, args.verbose)
    
    # The actual error has already been logged
    if not success:
        logger.warning("Failed to fetch radar data")
        sys.exit(1)
    
    if args.verbose:
        logger.info("Done!")
    sys.exit(0)


if __name__ == "__main__":
    main()
