import sys
import os
import types

# This runs BEFORE any imports in the frozen app
def inject_real_config():
    """Inject the real pyart_default_config into sys.modules before pyart loads"""
    
    config_path = os.path.join(sys._MEIPASS, 'pyart', 'pyart_default_config.py')
    
    if not os.path.exists(config_path):
        print(f"[HOOK ERROR] Config not found at {config_path}")
        return
    
    # Load the real config file as a module
    import importlib.util
    spec = importlib.util.spec_from_file_location('pyart.config.metadata_config', config_path)
    metadata_config = importlib.util.module_from_spec(spec)
    sys.modules['pyart.config.metadata_config'] = metadata_config
    spec.loader.exec_module(metadata_config)
    
    # Prevent pyart.config from trying to load default_config.py from disk
    sys.modules['pyart.default_config'] = metadata_config
    
    # print('[HOOK] Successfully injected pyart_default_config into sys.modules')
    # print(f"[HOOK] Loaded real pyart config from {config_path}")

inject_real_config()
