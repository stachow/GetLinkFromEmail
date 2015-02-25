# GetLinkFromEmail
A Windows Console App that reads the latest email sent to a given account (using IMAP), extracts a hyperlink from the body and presents the hyperlink in a browser.  Useful for e.g. Selenium testing where the test story involves an email being sent containing a confirmation link.

## To Use ##

* Rename `App.example.config` to `App.config`. 
* Set your email account details in the appSettings sections in `App.config`. 
* Run the console app
	* The app will for listen for an http request on a local port, default `http://localhost:8111`
	* A browser visits `http://localhost:8111`: the app will read the latest **unread** email for the account, try to find and extract the hyperlink within the email and return the hyperlink to the browser in the response body.
	* If no email is found, a holding response is sent back containing a `meta http-equiv='refresh'` tag.  This tag means the browser will repeat the request in N seconds.  This will continue _ad-inifinitum_.
	* Optionally a request of the form **http://localhost:8111/?_account+123@gmail.com_** can be passed.  In this case, only emails sent to the sub-address specified will be examined for the latest **unread** email.

## In Selenium world ##
 This allows you to write a test story where you can point your test command to the local address as an alternative to [manually open email program/site; wait for email to arrive; click on link; restart Selenium test].  

* Prior to the Selenium test being run, the Console App is started.
* The appropriate test command is defined to visit the designated listening url at the appropriate point.
* The Selenium test has a `waitForHyperlink`-type command that will hold until the app retrieves and presents the appropraite link. 


