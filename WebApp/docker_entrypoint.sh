#!/bin/bash
cd /pipeline/source/app/publish
echo "starting"
mkdir /pipeline/source/app/publish/tmp
mkdir /pipeline/source/app/publish/wwwroot/uploads
# export TEMP=/pipeline/source/app/publish/tmp
export COMPlus_EnableDiagnostics=0
export TMPDIR=/pipeline/source/app/publish/tmp
dotnet YAHCMS.dll
