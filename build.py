import os
import os.path
import shutil
import sys
from subprocess import check_call

VERSION = '0.2.2'
BUILD = 0
VERSION_INFO = 'beta'
CONFIG = 'Release'

def join(abs, rel):
    return os.path.abspath(os.path.join(abs, rel))

def run(path, *args):
    check_call([path] + list(args))

ROOT_DIR = os.path.abspath(os.path.dirname(__file__))
SOLUTION_FILE = join(ROOT_DIR, 'src/BadBehavior.sln')
NUNIT_PROJECT = join(ROOT_DIR, 'src/BadBehavior.Tests/BadBehavior.Tests.nunit')
OUTPUT_DIR = join(ROOT_DIR, 'output')
NUGET_BASE = join(OUTPUT_DIR, '.nuget')
PROJECT_ROOT = join(ROOT_DIR, 'src/BadBehavior')
PROJECT_BUILD = join(PROJECT_ROOT, 'bin/' + CONFIG)

MSBUILD = join(os.environ['WINDIR'], 'Microsoft.NET/Framework/v4.0.30319/MSBuild.exe')
NUNIT = join(ROOT_DIR, 'src/packages/NUnit.Runners.2.6.0.12051/tools/nunit-console.exe')
NUGET = join(ROOT_DIR, 'src/packages/NuGet.CommandLine.2.0.0/tools/NuGet.exe')

if os.path.isdir(OUTPUT_DIR):
    shutil.rmtree(OUTPUT_DIR)
os.makedirs(OUTPUT_DIR)

# Re-create the version number

if VERSION_INFO:
    INFORMATIONAL_VERSION = VERSION + '-' + VERSION_INFO
else:
    INFORMATIONAL_VERSION = VERSION

versionfile = '''
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyVersion("%(version)s")]
[assembly: AssemblyFileVersion("%(version)s")]
[assembly: AssemblyInformationalVersion("%(versioninfo)s")]
''' % { 'version' : VERSION + '.' + str(BUILD), 'versioninfo' : INFORMATIONAL_VERSION }

with open(join(ROOT_DIR, 'src/VersionInfo.cs'), 'w') as vf:
    vf.write(versionfile)

# Build the solution

run(MSBUILD, SOLUTION_FILE, '/p:Configuration=' + CONFIG, '/p:Platform=Any CPU', '/target:Clean,Build')

# Run the tests

run(NUNIT, NUNIT_PROJECT, '/config=' + CONFIG)

# Create the NuGet package

NUGET_LIB = join(NUGET_BASE, 'lib')
NUGET_CONTENT = join(NUGET_BASE, 'content')

shutil.copytree(PROJECT_BUILD, NUGET_LIB)
os.makedirs(NUGET_CONTENT)
shutil.copy(join(PROJECT_ROOT, 'BadBehavior.nuspec'), NUGET_BASE)
shutil.copy(join(PROJECT_ROOT, 'web.config.transform'), NUGET_CONTENT)
shutil.copy(join(PROJECT_ROOT, 'BadBehaviorConfigurator.cs.pp'), NUGET_CONTENT)

run(NUGET, 'pack',
    join(NUGET_BASE, 'BadBehavior.nuspec'),
    '-OutputDirectory', OUTPUT_DIR,
    '-Version', INFORMATIONAL_VERSION
)
shutil.rmtree(NUGET_BASE)