#!/bin/sh

set -e

cd typescript
make libtree-sitter-typescript.dylib
cp libtree-sitter-typescript.dylib ../
