# -*- mode: python ; coding: utf-8 -*-
import os
from PyInstaller.utils.hooks import collect_data_files

_spec_dir = os.path.dirname(os.path.abspath(SPEC))

from PyInstaller.utils.hooks import collect_data_files

datas = [
    ('default_config.py', 'pyart'),
]
datas += collect_data_files('pyart')
datas += collect_data_files('nexradaws')
datas += collect_data_files('cmweather')

a = Analysis(
    ['NexradParser.py'],
    pathex=[],
    binaries=[],
    datas=datas,
    hiddenimports=[
        'nexradaws',
        'nexradaws.resources',
    ],
    hookspath=['hooks'],
    hooksconfig={},
    runtime_hooks=[os.path.join(_spec_dir, 'hooks', 'runtime_hook_nexrad.py')],
    excludes=[],
    noarchive=False,
    optimize=0,
)
pyz = PYZ(a.pure)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.datas,
    [],
    name='NexradParser',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=True,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
)
