<tpl:section name="Content">
	<div id="container">
		<header class="background header-footer">
			<h1>Premotion Software Solutions CMS</h1>
		</header>
		<div id="cms-content">

			<div class="clearfix cms-layout cms-onecolumn">
				<div class="cms-layout-column center middle">
					{Block}
				</div>
			</div>
			
		</div>
		<footer class="background header-footer">
			<p>&copy; {FormatDate( Now(), 'yyyy' )} <a href="http://www.premotion.nl" title="Premotion Software Solutions" target="_blank">Premotion Software Solutions</a></p>
		</footer>
	</div>
</tpl:section>



<!-- login block -->
<tpl:section name="LoginBlock" field="Block">
	<section class="background white block signin">
		<header>
			<h1>Welcome</h1>
		</header>
		<div class="content">
			<p>Welcome to the Premotion Mansion CMS, please provide your credentials so I can authenticate you.</p>
			{Control}
		</div>
		<footer>
			<p>Forgot your password? Contact the Premotion support team.</p>
		</footer>
	</section>
</tpl:section>