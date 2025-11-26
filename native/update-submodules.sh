#!/bin/sh

set -e

cd `dirname "$0"`

for module in `ls -d tree-sitter*/ -1 | sed s'|/||'`
do
    echo Updating $module
    cd $module
    git checkout master
    git pull
    cd ..
done