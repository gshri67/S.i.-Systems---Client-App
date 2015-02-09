
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
	2, 'This is a newer Eula','20150105 1:25:39 PM'
);
INSERT INTO [dbo].[Eula](
	[Version],
	[Text],
	[PublishedDate]
)
VALUES
(
	3, 'End User License Agreement' + CHAR(13) + CHAR(10) +
'Last revised on February 15, 2014 ' + CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'This End User License Agreement (the "EULA") is a binding legal agreement between you, as an individual or entity, and S.i. Systems Partnership ("S.i."). By downloading, installing, or using this application for Android, iOS or other internet platform, as applicable (the "Software"), you agree to be bound by the terms of this EULA. If you do not agree to the EULA, do not check the "I accept the terms" box and do not use the Software. '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'You agree that installation or use of the Software signifies that you have read, understood, and agree to be bound by the EULA.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'The Software is provided to you under this EULA solely for your commercial use with regards to the corporate entity identified by your email which is also your login. Use of the Software or of the S.i. content, information, membership functionality, search, recruiting, or any other services (“S.i. Service”) within the organization your email is associated with is the only use of the Software allowed.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'1. Description of Software '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'The Software is a downloadable software application that enables you to access S.i. Alumni Knowledge Solution™ functionality directly from your Android, iPhone, iPad or other internet device supported by S.i. (“Device”).'+ CHAR(13) + CHAR(10) +
'You may download the Software whether or not you use the S.i. Service, but you must associate it with your organizational email account to enable its full functionality.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'2. License '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'S.i. hereby grants you, subject to the terms and conditions of this Agreement, a non-exclusive, non-transferable license to:'+ CHAR(13) + CHAR(10) +
'•	Use the Software for the advantage of the corporation associated with your login email; '+ CHAR(13) + CHAR(10) +
'•	Install or use the Software on only one Device; and '+ CHAR(13) + CHAR(10) +
'•	Make one copy of the Software in any machine readable form solely for back-up purposes, provided you reproduce the Software in its original form and with all proprietary notices on the back-up copy. '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'For clarity, the foregoing is not intended to prohibit you from installing and backing-up the Software for another Device on which you also agreed to the EULA. Each instance of this EULA that you agree to grants you the aforementioned rights in connection with the installation, use and back-up of one copy of the Software on one Device.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'3. Title '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'Title, ownership and all rights (including without limitation intellectual property rights) in and to the Software shall remain with S.i. . Except for those rights expressly granted in this EULA, no other rights are granted, whether express or implied.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'4. Restrictions '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'You understand and agree that you shall only use the Software in a manner that complies with any and all applicable laws in the jurisdictions in which you use the Software. Your use shall be in accordance with applicable restrictions concerning privacy and intellectual property rights.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'You may not:'+ CHAR(13) + CHAR(10) +
'•	Create derivative works based on the Software; '+ CHAR(13) + CHAR(10) +
'•	Use the software for any purpose other than as described herein; '+ CHAR(13) + CHAR(10) +
'•	Copy or reproduce the Software except as described in this EULA; '+ CHAR(13) + CHAR(10) +
'•	Sell, assign, license, disclose, distribute or otherwise transfer or make available the Software or any copies of the Software in any form to any third parties; '+ CHAR(13) + CHAR(10) +
'•	Alter, translate, decompile, reverse assemble or reverse engineer the Software, or attempt to do any of the foregoing, except to the extent this prohibition is not permitted under an applicable law; or '+ CHAR(13) + CHAR(10) +
'•	Remove or alter any proprietary notices or marks on the Software. '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'5. Personal Information and Privacy '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'We may ask you to provide certain information about you during the Software downloading process. All personal information that you provide to us will be governed by the S.i. Privacy Policy. You may use the Software whether or not you use the S.i. Service. If you use the S.i. Service, the personal information you provide to S.i.  will also be governed by the Privacy Policy. You understand and agree that S.i.  may disclose information if required to do so by law or in the good faith belief that such disclosure is reasonably necessary to comply with legal process, enforce the terms of this EULA, or protect the rights, property, or safety of S.i., its clients and consultant candidates, or the public.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'6. No Warranty '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'S.I.  DOES NOT WARRANT THAT THE FUNCTIONS CONTAINED IN THE SOFTWARE WILL MEET ANY REQUIREMENTS OR NEEDS YOU MAY HAVE, OR THAT THE SOFTWARE WILL OPERATE ERROR FREE, OR IN AN UNINTERRUPTED MANNER, OR THAT ANY DEFECTS OR ERRORS WILL BE CORRECTED, OR THAT THE SOFTWARE IS FULLY COMPATIBLE WITH ANY PARTICULAR PLATFORM. THE SOFTWARE IS OFFERED ON AN "AS-IS" BASIS AND NO WARRANTY, EITHER EXPRESS OR IMPLIED, IS GIVEN. S.I.  EXPRESSLY DISCLAIMS ALL WARRANTIES OF ANY KIND, WHETHER EXPRESS OR IMPLIED, INCLUDING, BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. SOME JURISDICTIONS DO NOT ALLOW THE WAIVER OR EXCLUSION OF IMPLIED WARRANTIES SO THEY MAY NOT APPLY TO YOU.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'7. Right to Terminate or Modify Software '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'S.i.  may modify the Software and this EULA with notice to you either in email or by publishing notice on the Website, including but not limited to charging fees for the Software, or changing the functionality or appearance of the Software. In the event S.i.  modifies the Software or the EULA, you may terminate this EULA and cease use of the Software. S.i.  may terminate your use of the Software, the EULA or the S.i.  Service at any time, with or without notice.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'8. Indemnification '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'By accepting the EULA, you agree to indemnify and otherwise hold harmless S.i.  Partnership, its officers, employers, agents, subsidiaries, affiliates and other partners from any direct, indirect, incidental, special, consequential or exemplary damages arising out of, relating to, or resulting from your use of the Software or any other matter relating to the Software.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'9. Limitation of Liability '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'YOU EXPRESSLY UNDERSTAND AND AGREE THAT S.I.  SHALL NOT BE LIABLE FOR ANY INDIRECT, INCIDENTAL, SPECIAL, CONSEQUENTIAL OR EXEMPLARY DAMAGES, INCLUDING BUT NOT LIMITED TO, DAMAGES FOR LOSS OF PROFITS, GOODWILL, USE, DATA OR OTHER INTANGIBLE LOSSES (EVEN IF S.I.  HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES). IN NO EVENT WILL S.I. ''S AGGREGATE LIABILITY TO YOU EXCEED THE AMOUNT OF LICENSING FEES PAID BY YOU TO S.I. . THESE LIMITATIONS AND EXCLUSIONS WILL APPLY NOTWITHSTANDING ANY FAILURE OF ESSENTIAL PURPOSE OF ANY LIMITED REMEDY. SOME JURISDICTIONS DO NOT ALLOW THE LIMITATIONS OF DAMAGES AND/OR EXCLUSIONS OF LIABILITY FOR INCIDENTAL OR CONSEQUENTIAL DAMAGES. ACCORDINGLY, SOME OF THE ABOVE LIMITATIONS MAY NOT APPLY TO YOU.'+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'10. General '+ CHAR(13) + CHAR(10) +
+ CHAR(13) + CHAR(10) +
'The EULA between you and S.i. Partnership will be governed by and construed in accordance with the laws of the Province of Alberta without regard to conflict of laws principles. The exclusive forum for any disputes arising out of or relating to this EULA shall be an appropriate court sitting in the City of Calgary Alberta. The EULA constitutes the entire agreement between you and S.i.  regarding the Software. If any provision of this EULA is held by a court of competent jurisdiction to be contrary to law, such provision will be changed and interpreted so as to best accomplish the objectives of the original provision to the fullest extent allowed by law and the remaining provisions of this EULA will remain in full force and effect. You may not assign this EULA, and any assignment of this EULA by you will be null and void. You agree not to display or use S.i. trademarks in any manner without S.i. ''s prior, written permission. The section titles and numbering of this EULA are displayed for convenience and have no legal effect.',
'20150109 3:22:12 PM');

SET IDENTITY_INSERT [dbo].[Eula] OFF;