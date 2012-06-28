import os
import os.path
import sys
from subprocess import check_call

VERSION = '0.0.0.0'
CONFIG = 'Release'

def join(abs, rel):
    return os.path.abspath(os.path.join(abs, rel))

def run(path, *args):
    check_call([path] + list(args))

ROOT_DIR = os.path.abspath(os.path.dirname(__file__))
SOLUTION_FILE = join(ROOT_DIR, 'src/BadBehavior.sln')
NUNIT_PROJECT = join(ROOT_DIR, 'src/BadBehavior.Tests/BadBehavior.Tests.nunit')
OUTPUT_DIR = join(ROOT_DIR, 'output')

MSBUILD = join(os.environ['WINDIR'], 'Microsoft.NET/Framework/v4.0.30319/MSBuild.exe')
NUNIT = join(ROOT_DIR, 'src/packages/NUnit.Runners.2.6.0.12051/tools/nunit-console.exe')

if not os.path.isdir(OUTPUT_DIR):
    os.makedirs(OUTPUT_DIR)

# Build the solution

run(MSBUILD, SOLUTION_FILE, '/p:Configuration=' + CONFIG, '/p:Platform=Any CPU', '/target:Clean,Build')

# Run the tests

run(NUNIT, NUNIT_PROJECT, '/config=' + CONFIG)
