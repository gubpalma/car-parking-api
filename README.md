# car-parking-api

When compiled and run locally, API host will listen on http://localhost:5000

You can test the API by:

* Send a HTTP POST to ```api/parking/enter``` with the 'car registration' (any string). This 'stores' the car registration in a mock repository.
* Send a HTTP POST to ```api/parking/checkout``` with the 'car registration' (a previously 'entered' registration). This will create an invoice based on the current time and the date the car entered.
* In an actual system, the payment system would send a HTTP POST to ```api/parking/exit``` with the registration to stamp an actual exit time on the entry. This is more for record keeping and has no practical effect (although it prevents the same car from being incorrectly billed as the API looks for the current 'un-exited' parking entry).
