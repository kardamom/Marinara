# Marinara
A Grasshopper component library to generate the pasta forms mathematically defined in George L Legendre's 2011 book _Pasta By Design_. Also included are some of the forms from Joseph Choma's 2015 book _Morphing_.

This is an ongoing student research project for Spring Semester 2024.

## Implemented pasta

* Radiatori
* Casarecce
* Fusilli Capri
* Lancette
* Scialatielli
* Fiocchi Rigati
* Agnolotti
* Riccioli Ai Cinque Sapori

## Installation
Copy [Marinara.gha ](https://github.com/kardamom/Marinara/releases/) to your Grasshopper components folder. Restart Rhino.

The library should appear under in Grasshopper under a new tab, 'Marinara'.

## Usage
The default values are enough to generate a pasta form.

### Input Parameters

* U domain - domain of values for U
* V domain - domain of values for V
* steps - how many points should we make in each of U and V
* radius/plumpness/etc - each pasta has at least one additional parameter to manipulate the form

### Outputs

* Surface
* UCurves
* VCurves
* U
* V


## Samples
![6 up](https://raw.githubusercontent.com/kardamom/Marinara/master/Docs/Pasta-6up.png)

