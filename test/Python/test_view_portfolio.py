#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

class ViewPortfolioTestCase(unittest.TestCase):
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

	def test_view_portfolio_success(self):
		"""An authenticated user can sell shares that they own"""
		expected_response_code = 200
		expected_keys = ["id", "name", "balance", "positions"]
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
		portfolio_response = ApiFacade.get_portfolio(token, account_id)

		deletion_response = ApiFacade.delete_user(token)

		self.assertEqual(portfolio_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}"
			.format(expected_response_code, portfolio_response.get_http_status()))

		for k in expected_keys:
			self.assertEqual(k in portfolio_response.get_json_body(), True, msg = "Expected key [{0}]; got [{1}]".format(k, str(None)))

if __name__ == "__main__":
	unittest.main()