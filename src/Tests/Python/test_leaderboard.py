#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

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

	def test_view_leaderboard_success(self):
		expected_response_code = 200
		expected_keys_in_leaderboard = ["items", "pageNumber", "pageSize", "totalPageCount", "totalRowCount"]
		expected_keys_in_leaderboard_items = ["rank", "displayName", "totalAccountValue", "profit", "profitPercent", "isCurrentUser"]
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		leaderboard_response = ApiFacade.get_leaderboard(authentication_response.get_token())
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		self.assertEqual(leaderboard_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, leaderboard_response.get_http_status()))

		for k in expected_keys_in_leaderboard:
			exists = k in leaderboard_response.get_json_body()
			self.assertEqual(exists, True, msg = "Expected value for key [{0}]; got [{1}]".format(k, None))

		for item in leaderboard_response.get_all_items():
			for k in expected_keys_in_leaderboard_items:
				exists = k in item
				self.assertEqual(exists, True, msg = "Expected value for key [{0}]; got [{1}]".format(k, None))

if __name__ == "__main__":
	unittest.main()