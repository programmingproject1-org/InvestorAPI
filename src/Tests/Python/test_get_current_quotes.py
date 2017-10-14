#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade


TEST_USER = {
			"displayName": "John Doe", 
			"email": "johndoe@test.com", 
			"password": "12345678",
			"level": "Investor",
			"accounts": [
					{
					"name": "Default Account", 
					"balance": 1000000
					}
			],
			"watchlists": [
					{
					"name": "Default Watchlist"
					}
			]
		}

TOKEN = None

@ddt
class CurrentQuotesTestCase(unittest.TestCase):
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
	def test_get_single_quote_success(self, symbol):
		expected_response_code = 200
		no_of_quotes = 1
		expected_keys_in_quote = ["symbol", "ask", "askSize", "bid", "bidSize", "last", "lastSize", "change", "changePercent", "dayLow", "dayHigh"]
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		currentquotes_response = ApiFacade.get_current_quotes(authentication_response.get_token(), symbol)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		self.assertEqual(currentquotes_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, currentquotes_response.get_http_status()))

		self.assertEqual(len(currentquotes_response.get_all_quotes()), no_of_quotes, 
			msg = "Expected {0} quotes; got {1} quotes".format(no_of_quotes, len(currentquotes_response.get_all_quotes())))

		for quote in currentquotes_response.get_all_quotes():
			for k in expected_keys_in_quote:
				self.assertIsNotNone(quote[k], msg = "Expected value for key [{0}]; got [{1}]".format(k, None))

	@file_data("data/current_quotes/multiple_symbols_success.json")
	def test_get_multiple_quotes_success(self, symbols):
		expected_response_code = 200
		no_of_quotes = len(symbols)
		expected_keys_in_quote = ["symbol", "ask", "askSize", "bid", "bidSize", "last", "lastSize", "change", "changePercent", "dayLow", "dayHigh"]
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		currentquotes_response = ApiFacade.get_current_quotes(authentication_response.get_token(), symbols)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		self.assertEqual(currentquotes_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, currentquotes_response.get_http_status()))

		self.assertEqual(len(currentquotes_response.get_all_quotes()), no_of_quotes, 
			msg = "Expected {0} quotes; got {1} quotes".format(no_of_quotes, len(currentquotes_response.get_all_quotes())))

		for quote in currentquotes_response.get_all_quotes():
			for k in expected_keys_in_quote:
				self.assertIsNotNone(quote[k], msg = "Expected value for key [{0}]; got [{1}]".format(k, None))

if __name__ == "__main__":
	unittest.main()