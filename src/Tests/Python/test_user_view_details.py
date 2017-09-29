#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade

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

class UserViewDetailsTestCase(unittest.TestCase):
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

	def validate_details(self, json_details, expected_details):
		if json_details is None: return
		errors = []

		user_keys = ["displayName", "email", "level"]
		for key in user_keys:
			if key not in json_details:
				errors.append("Missing key: {0}".format(key))
			else:
				if json_details[key] != expected_details[key]:
					errors.append("Value for [{0}] expected to be {1} but got {2} instead".format(key, expected_details[key], json_details[key]))

		if "accounts" not in json_details:
			errors.append("Missing key: accounts")
			return

		account = json_details["accounts"][0]
		expected_account = expected_details["accounts"][0]
		account_keys = ["name", "balance"]
		for key in account_keys:
			if key not in account:
				errors.append("Missing key: {0}".format(key))
			else:
				if account[key] != expected_account[key]:
					errors.append("Value for [{0}] expected to be {1} but got {2} instead".format(key, expected_account[key], account[key]))

		if "watchlists" not in json_details:
			errors.append("Missing key: watchlists")
			return

		watchlist = json_details["watchlists"][0]
		expected_watchlist = expected_details["watchlists"][0]
		watchlist_keys = ["name"]
		for key in watchlist_keys:
			if key not in watchlist:
				errors.append("Missing key: {0}".format(key))
			else:
				if watchlist[key] != expected_watchlist[key]:
					errors.append("Value for [{0}] expected to be {1} but got {2} instead".format(key, expected_watchlist[key], watchlist[key]))

		if len(errors) != 0:
			return (False, errors)

		return (True, None)


	def test_viewDetails_success(self):
		"""An authenticated user can view their user account details"""
		expected_status = 200
		api = ApiFacade()
		response_code, json_details = api.view_details(TOKEN)
		self.assertEqual(response_code, expected_status, msg = "Expected [HTTP {0}]; Got [HTTP - {1}]".format(expected_status, response_code))
		
		is_success, errors = self.validate_details(json_details, TEST_USER)
		self.assertEqual(is_success, True, msg = "Failed: {0}; Got: [HTTP {1}]; Expected: [HTTP {2}]"
			.format(errors, response_code, expected_status))

	def test_deletion_userIsNotAuthenticated(self):
		"""An unauthenticated user cannot delete their user account"""
		expected_status = 401
		api = ApiFacade()
		response_code, json_details = api.view_details(None)
		self.assertEqual(response_code, expected_status, msg = "Expected [HTTP {0}]; Got [HTTP - {1}]".format(expected_status, response_code))

if __name__ == "__main__":
	unittest.main()