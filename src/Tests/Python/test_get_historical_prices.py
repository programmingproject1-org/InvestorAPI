#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

@ddt
class HistoricalPricesTestCase(unittest.TestCase):
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
		expected_keys_in_item = ["timestamp", "open", "high", "low", "close"]
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		historicalprices_response = ApiFacade.get_historical_prices(authentication_response.get_token(), symbol)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		self.assertEqual(historicalprices_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, historicalprices_response.get_http_status()))

		for quote in historicalprices_response.get_all_data_points():
			for k in expected_keys_in_item:
				self.assertIsNotNone(quote[k], msg = "Expected value for key [{0}]; got [{1}]".format(k, None))

if __name__ == "__main__":
	unittest.main()