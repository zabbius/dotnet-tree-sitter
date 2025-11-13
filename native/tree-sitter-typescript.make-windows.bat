@echo off

cd typescript

del Makefile
copy ..\..\Makefile.windows.tree-sitter-grammar Makefile
nmake tree-sitter-typescript.dll GRAMMAR=typescript

cd ..
copy typescript\tree-sitter-typescript.dll .

