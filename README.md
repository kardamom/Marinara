# Marinara
A Grasshopper component library to generate the pasta forms mathematically defined in George L Legendre's 2011 book _Pasta By Design_. Also included are some of the forms from Joseph Choma's 2015 book _Morphing_.

This is a student research project for Spring Semester 2024. I am Master of Architecture candidate at the University of Southern California. My main aim is to learn C#, further my understanding of computational geometry, and learn about the internals of Grasshopper and Rhino.

This first stage is all about creating digital surfaces and UV curves representing the pasta, and from there drawings. Later on I will attempt to make physical models of the forms, avoiding 3d printing as much as possible. I will also investigate how these forms could be applied to architecture.

## Implemented pasta

* Radiatori
* Casarecce
* Fusilli Capri
* Lancette
* Scialatielli
* Fiocchi Rigati

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

