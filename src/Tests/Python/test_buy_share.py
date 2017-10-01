#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

class BuyShareTestCase(unittest.TestCase):
	@classmethod
	def setUpClass(cls):
		pass

	@classmethod
	def tearDownClass(cls):
		pass

	def setUp(self):
		pass

	def tearDown(self):
		pass

	def test_buy_shares_success(self):
		expected_response_code = 200
		symbol_to_buy = "ANZ" 
		quantity = 100
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		token = authentication_response.get_token()

		# get account id
		viewdetails_response = ApiFacade.view_details(token)
		account_id = viewdetails_response.get_main_account_id()

		buyshare_response = ApiFacade.buy_share(token, account_id, symbol_to_buy, quantity)

		self.assertEqual(buyshare_response.get_http_status(), 200, msg = "Not 200 OK")

if __name__ == "__main__":
	unittest.main()