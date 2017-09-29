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

	def validate_response(self, json_data):
		if json_data is None: return (False, ["Response was none"])
		errors = []
		keys = ["symbol", "ask", "askSize", "bid", "bidSize", "last", "lastSize", "change", "changePercent", "dayLow", "dayHigh"]
		for key in keys:
			if key not in json_data[0]:
				errors.append("Missing key: {0}".format(key))
				print(key)

		if len(errors) == 0:
			is_success = True
		else:
			is_success = False

		return (is_success, errors)

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

	@file_data("data/current_quotes/success.json")
	def test_get_single_quote_success(self, companyName, symbol, industry):
		global TOKEN
		api = ApiFacade()
		response = api.get_current_quotes(symbol, TOKEN)

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

		validator = ResponseValidator(response, 200, model)
		self.assertEqual(validator.response_code_success(), True, msg = validator.get_errors())
		self.assertEqual(validator.response_body_success(), True, msg = validator.get_errors())
		return 
if __name__ == "__main__":
	unittest.main()