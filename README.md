euthenias-delight
=================

An invoice generator (bonus points if you get the reference in the repository name.)

Accepts an XML file from a client who specify the job that needs to be invoiced.
The XML file is of the form:

	<invoice>
	  <to>
		<customer>
		  <name>
			<singleName></singleName>
		  </name>
		  <address>
			<line1></line1>
			<city></city>
			<postalCode></postalCode>
		  </address>
		  <attention></attention>
		  <email></email>
		  <nickname></nickname>
		</customer>
	  </to>
	  <items>
		<!--As many item elements as you need-->
		<item>
		  <itemNumber></itemNumber>
		  <description></description>
		  <quantity></quantity>
		  <price></price>
		  <discount></discount>
		</item>
	  <terms></terms>
	  <gst>
		<collecting></collecting>
		<percent></percent>
	  </gst>
	  <notes>
		<!--As many note elements as you need-->
		<note></note>
	  </notes>
	  <emailBody>
		<greeting></greeting>
		<message></message>
		<salutation></salutation>
	  </emailBody>
	 </invoice>