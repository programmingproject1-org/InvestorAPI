#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

@ddt
class FundamentalsTestCase(unittest.TestCase):
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

	@file_data("data/current_quotes/single_symbol_success.json")
	def test_get_historical_prices_success(self, symbol):
		expected_response_code = 200
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		dividends_response = ApiFacade.get_dividends(authentication_response.get_token(), 
			symbol)
		
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		self.assertEqual(dividends_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, dividends_response.get_http_status()))

if __name__ == "__main__":
	unittest.main()