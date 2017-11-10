#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

class ResetAccountTestCase(unittest.TestCase):
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

	def test_reset_account_success(self):
		"""An authenticated user can reset their account"""
		expected_response_code = 201
		symbol = "DDD" 
		quantity = 100
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		token = authentication_response.get_token()

		# get account id
		viewdetails_response = ApiFacade.view_details(token)
		account_id = viewdetails_response.get_main_account_id()

		# buy shares first
		buyshare_response = ApiFacade.buy_share(token, account_id, symbol, int(quantity))
		
		resetaccount_response = ApiFacade.reset_account(token, account_id)
		updatedportfolio_response = ApiFacade.get_portfolio(token, account_id)
		
		deletion_response = ApiFacade.delete_user(token)

		self.assertEqual(resetaccount_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}"
			.format(expected_response_code, resetaccount_response.get_http_status()))

if __name__ == "__main__":
	unittest.main()