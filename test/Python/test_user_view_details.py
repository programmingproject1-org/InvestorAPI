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

class UserViewDetailsTestCase(unittest.TestCase):
	@classmethod
	def setUpClass(cls):
		pass
		# global TOKEN
		# cls.api = ApiFacade()
		# response_code = cls.api.register_user(TEST_USER["displayName"], TEST_USER["email"], TEST_USER["password"])
		# response_code, TOKEN = cls.api.authenticate_user(TEST_USER["email"], TEST_USER["password"])

	@classmethod
	def tearDownClass(cls):
		pass
		# global TOKEN
		# cls.api.delete_user(TOKEN)

	def setUp(self):
		pass

	def tearDown(self):
		pass

	def test_viewDetails_success(self):
		"""An authenticated user can view their user account details"""
		expected_response_code = 200
		displayName, email, password, level = ("John Doe", "johndoe@test.com", "12345678", "Investor")
		number_of_accounts, number_of_watchlists = (1, 1)

		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		viewdetails_response = ApiFacade.view_details(authentication_response.get_token())
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		self.assertEqual(viewdetails_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, viewdetails_response.get_http_status()))
		
		self.assertEqual(viewdetails_response.get_displayName(), displayName,
			msg = "Expected displayName = {0}; got displayName = {1}".format(displayName, viewdetails_response.get_displayName()))
		
		self.assertEqual(viewdetails_response.get_email(), email,
			msg = "Expected email = {0}; got email = {1}".format(email, viewdetails_response.get_email()))
		
		self.assertEqual(viewdetails_response.get_level(), level,
			msg = "Expected level = {0}; got level = {1}".format(level, viewdetails_response.get_level()))
		
		self.assertEqual(len(viewdetails_response.get_accounts()), number_of_accounts,
			msg = "Expected len(accounts) = {0}; got len(accounts) = {1}"
			.format(number_of_accounts, len(viewdetails_response.get_accounts())))

		self.assertEqual(len(viewdetails_response.get_watchlists()), number_of_watchlists,
			msg = "Expected len(watchlists) = {0}; got len(watchlists) = {1}"
			.format(number_of_watchlists, len(viewdetails_response.get_watchlists())))

	def test_deletion_userIsNotAuthenticated(self):
		"""An unauthenticated user cannot delete their user account"""
		expected_response_code = 401
		expected_response_body = None

		viewdetails_response = ApiFacade.view_details(token = None)

		self.assertEqual(viewdetails_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, viewdetails_response.get_http_status()))

		self.assertEqual(viewdetails_response.get_json_body(), expected_response_body, 
			msg = "Expected body = {0}; got body = {1}".format(expected_response_body, viewdetails_response.get_json_body()))

if __name__ == "__main__":
	unittest.main()