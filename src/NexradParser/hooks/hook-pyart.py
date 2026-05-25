from PyInstaller.utils.hooks import collect_data_files, get_module_file_attribute
import os
import shutil

datas = collect_data_files('pyart')

# Add default_config.py explicitly
project_root = os.path.dirname(os.path.dirname(__file__))
config_src = os.path.join(project_root, 'pyart_default_config.py')

# Remove any existing pyart/default_config.py from datas
datas = [(d, dest) for d, dest in datas if not d.endswith('default_config.py')]

# Add our copied version
datas.append((config_src, 'pyart'))

hiddenimports = [
    'pyart.io.nexrad_common',
    'pyart.io.nexrad',
]
