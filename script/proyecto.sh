#!/bin/bash

 
run() {
cd ..
make dev
cd script
}
report() {
cd ..
cd informe
pdflatex informe.tex
cd ..
cd script
}
slides() {
cd ..
cd presentacion
pdflatex Presentacion.tex
cd ..
cd script
}
function show_report() {
    local viewer="$1"
    case "$viewer" in
        evince) evince informe.pdf 
;;
        okular) okular informe.pdf 
;;
        xpdf) xpdf informe.pdf 
;;
        *) okular informe.pdf
;;

esac
}
function show_slides() {
    local viewer="$1"
    case "$viewer" in
        evince) evince Presentacion.pdf 
;;
        okular) okular Presentacion.pdf 
;;
        xpdf) xpdf Presentacion.pdf 
;;
        *) okular Presentacion.pdf
;;

esac
}
clean() {
cd ..
rm */*
git reset --hard
cd script
}