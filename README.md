<h1>Marinara</h1>
<p>A Grasshopper component library to generate the pasta forms mathematically defined in George L Legendre's 2011 book <i>Pasta By Design</i>. Also included are some of the forms from Joseph Choma's 2015 book <i>Morphing</i>.</p>

<p>This is a student research project for Spring Semester 2024. I am Master of Architecture candidate at the University of Southern California. My main aim is to learn C#, further my understanding of computational geometry, and learn about the internals of Grasshopper and Rhino.</p>
<p>This first stage is all about creating digital surfaces and UV curves representing the pasta, and from there drawings. Later on I will attempt to make physical models of the forms, avoiding 3d printing as much as possible. I will also investigate how these forms could be applied to architecture.</p>

<h2>Implemented pasta</h2>
<ul>
<li>Radiatori</li>
  <li>Casarecce</li>
  <li>Fusilli Capri</li>
  <li>Lancette</li>
  <li>Scialatielli</li>
  <li>Fiocchi Rigati</li>
</ul>

<h2>Installation</h2>
  <p>
    
  <br/>
  The library should appear under a new tab, 'Marinara', in Grasshopper.
</p>
<h2>Usage</h2>
<p>The default values are enough to generate a pasta form.<p>
  <h3>Input Parameters</h3>
<ul>
<li>U domain - domain of values for U</li>
<li>V domain - domain of values for V</li>
<li>steps - how many points should we make in each of U and V</li>
  <li>radius/plumpness/etc - each pasta has at least one additional parameter to manipulate the form</li>
</ul>
<h3>Outputs</h3>
<ul>
<li>Surface</li>
<li>UCurves</li>
<li>VCurves</li>
  <li>U</li>
  <li>V</li>
</ul>
