euthenias-delight
=================

An invoice generator

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
		<item>
		  <itemNumber></itemNumber>
		  <description></description>
		  <quantity></quantity>
		  <price></price>
		  <discount></discount>
		</item>
		<item>
		  <itemNumber></itemNumber>
		  <description></description>
		  <quantity></quantity>
		  <price></price>
		  <discount></discount>
		</item>
	  </items>
	  <terms></terms>
	  <gst>
		<collecting></collecting>
		<percent></percent>
	  </gst>
	  <notes>
		<note></note>
		<note></note>
		<note></note>
	  </notes>
	  <emailBody>
		<greeting></greeting>
		<message></message>
		<salutation></salutation>
	  </emailBody>
	 </invoice>