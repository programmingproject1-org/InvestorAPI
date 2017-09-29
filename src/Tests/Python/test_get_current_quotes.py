#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade
from models.response_validator import ResponseValidator


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
		global TOKEN
		cls.api = ApiFacade()
		response_code = cls.api.register_user(TEST_USER["displayName"], TEST_USER["email"], TEST_USER["password"])
		response_code, TOKEN = cls.api.authenticate_user(TEST_USER["email"], TEST_USER["password"])

	@classmethod
	def tearDownClass(cls):
		global TOKEN
		cls.api.delete_user(TOKEN)

	def setUp(self):
		pass

	def tearDown(self):
		pass

	@file_data("data/current_quotes/singe_symbol_success.json")
	def test_get_single_quote_success(self, symbol):
		global TOKEN
		api = ApiFacade()
		response = api.get_current_quotes(symbol, TOKEN)
		expected_response_code = 200
		model = {
			"key_only": False,
			"is_collection": True,
			"accept_empty": False,
			"model": {
				"symbol": {"key_only": True, "is_collection": False},
				"ask": {"key_only": True, "is_collection": False},
				"askSize": {"key_only": True, "is_collection": False},
				"bid": {"key_only": True, "is_collection": False},
				"bidSize": {"key_only": True, "is_collection": False},
				"last": {"key_only": True, "is_collection": False},
				"lastSize": {"key_only": True, "is_collection": False},
				"change": {"key_only": True, "is_collection": False},
				"changePercent": {"key_only": True, "is_collection": False},
				"dayLow": {"key_only": True, "is_collection": False},
				"dayHigh": {"key_only": True, "is_collection": False}
			}
		}

		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()
		self.assertEqual(correct_status, True, msg = "On Symbol [{0}] - {1}".format(symbol, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On Symbol [{0}] - {1}".format(symbol, validator.get_errors()))

	@file_data("data/current_quotes/multiple_symbols_success.json")
	def test_get_multiple_quotes_success(self, symbols):
		global TOKEN
		api = ApiFacade()
		response = api.get_current_quotes(symbols, TOKEN)
		expected_response_code = 200
		symbols_list = symbols.split(',')
		model = {
			"key_only": False,
			"is_collection": True,
			"accept_empty": False,
			"model": {
				"symbol": {"key_only": False, "is_collection": False, "value": symbols_list},
				"ask": {"key_only": True, "is_collection": False},
				"askSize": {"key_only": True, "is_collection": False},
				"bid": {"key_only": True, "is_collection": False},
				"bidSize": {"key_only": True, "is_collection": False},
				"last": {"key_only": True, "is_collection": False},
				"lastSize": {"key_only": True, "is_collection": False},
				"change": {"key_only": True, "is_collection": False},
				"changePercent": {"key_only": True, "is_collection": False},
				"dayLow": {"key_only": True, "is_collection": False},
				"dayHigh": {"key_only": True, "is_collection": False}
			}
		}

		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()
		self.assertEqual(correct_status, True, msg = "On Symbol [{0}] - {1}".format(symbols, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On Symbol [{0}] - {1}".format(symbols, validator.get_errors()))

if __name__ == "__main__":
	unittest.main()