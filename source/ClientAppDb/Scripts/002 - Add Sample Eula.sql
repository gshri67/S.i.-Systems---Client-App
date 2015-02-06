
SET IDENTITY_INSERT [dbo].[Eula] ON;

-- Adds some sample EULAs
INSERT INTO [dbo].[Eula](
	[Version],
	[Text],
	[PublishedDate]
)
VALUES
(
	1, 'This is an old Eula','20150101 10:34:09 AM'
);
INSERT INTO [dbo].[Eula](
	[Version],
	[Text],
	[PublishedDate]
)
VALUES
(
	2, 'This is a new Eula ' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + 
	'Vegan 3 wolf moon before they sold out Portland, Echo Park keffiyeh distillery. Messenger bag gluten-free fingerstache viral tote bag, Schlitz cred Godard next level four dollar toast flannel fashion axe normcore. 8-bit biodiesel keffiyeh crucifix. Pork belly authentic Neutra, readymade Shoreditch Banksy organic locavore fanny pack swag crucifix you probably havent heard of them hoodie. Asymmetrical banh mi blog forage, food truck selvage photo booth. Vegan whatever church-key, paleo freegan photo booth meditation migas lo-fi small batch roof party hoodie. Letterpress mixtape single-origin coffee, Truffaut Etsy ugh organic Banksy. ' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + 
	'Food truck ugh Echo Park, trust fund beard raw denim wayfarers biodiesel. Cornhole single-origin coffee crucifix wayfarers, ugh iPhone artisan sartorial. Shabby chic bicycle rights leggings gastropub biodiesel. Fanny pack trust fund heirloom, biodiesel twee banh mi Intelligentsia listicle Helvetica kitsch scenester Williamsburg migas kogi letterpress. Pickled Bushwick mumblecore skateboard. Meh leggings Brooklyn pickled. VHS 3 wolf moon taxidermy, Etsy iPhone letterpress Portland tote bag biodiesel cornhole.' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) +
	'Jean shorts drinking vinegar whatever cornhole cardigan bicycle rights. Banh mi fixie Godard, single-origin coffee High Life chambray 90s Neutra pour-over blog occupy photo booth Pinterest XOXO PBR. Fap cray PBR&B vinyl freegan, Neutra salvia. Thundercats authentic chia, lomo dreamcatcher butcher Helvetica Godard listicle flannel health goth. Sartorial flannel bespoke vinyl. Cold-pressed lo-fi plaid, Bushwick XOXO whatever Pinterest Banksy fixie bicycle rights selfies literally. Church-key raw denim occupy Pitchfork biodiesel.',
	'20150105 1:25:39 PM'
);

SET IDENTITY_INSERT [dbo].[Eula] OFF;