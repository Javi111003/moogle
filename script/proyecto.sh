#!/bin/bash

 funcion=$1
viewer=$2

case $funcion in

run)
cd ..
make dev
cd script
;;
report)
cd ..
cd informe
pdflatex informe.tex
cd ..
cd script
;;
slides)
cd ..
cd presentacion
pdflatex Presentacion.tex
cd ..
cd script
;;
show_report)
cd ..
cd informe
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

;;
show_slides)
cd ..
cd presentacion
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

;;

clean)
cd ..
rm */*
git reset --hard
cd script
;;

*) echo "Elija una opcion válida"
;;

esac
