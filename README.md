euthenias-delight
=================

An invoice generator (bonus points if you get the reference in the repository name.)

Accepts an XML file from a client that specifies the details about the job to be invoiced.
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
	  <currency></currency>
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
	 
Example Data:
	<invoice>
	  <to>
		<customer>
		  <name>
			<singleName>CUSTOMER COMPANY NAME</singleName>
		  </name>
		  <address>
			<line1>L10 / 10 SMITH STREET</line1>
			<city>MELBOURNE VIC</city>
			<postalCode>3000</postalCode>
		  </address>
		  <attention>Joseph Blow</attention>
		  <email>jblow@customercompany.com</email>
		  <nickname>Joe</nickname>
		</customer>
	  </to>
	  <items>
		<item>
		  <itemNumber>1</itemNumber>
		  <description>Description of task 1</description>
		  <quantity>1</quantity>
		  <price>100</price>
		  <discount>0</discount>
		</item>
		<item>
		  <itemNumber>2</itemNumber>
		  <description>Description of task 2</description>
		  <quantity>1</quantity>
		  <price>0</price>
		  <discount>0</discount>
		</item>
	  </items>
	  <terms>7 Days</terms>
	  <gst>
		<collecting>false</collecting>
		<percent>10</percent>
	  </gst>
	  <currency>AUD</currency>
	  <notes>
		<note>Please remit to:</note>
		<note>Me</note>
		<note>EFT to BankName BSB XXX-YYY Account NNNNNNNNN</note>
	  </notes>
	  <emailBody>
		<greeting>Hi </greeting>
		<message>Please find invoice attached.</message>
		<salutation>Regards, Me</salutation>
	  </emailBody>
	 </invoice>