#!/bin/sh

set -e

cd typescript
make libtree-sitter-typescript.so
cp libtree-sitter-typescript.so ../
