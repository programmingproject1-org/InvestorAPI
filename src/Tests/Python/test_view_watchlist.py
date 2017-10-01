#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

@ddt
class ViewWatchlistTestCase(unittest.TestCase):
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

	def test_get_watchlist_success(self):
		expected_response_code = 200
		expected_keys_in_quote = ["symbol", "name", "lastPrice", "change", "changePercent"]
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		token = authentication_response.get_token()

		# get watchlist id
		viewdetails_response = ApiFacade.view_details(token)
		watchlist_id = viewdetails_response.get_main_watchlist_id()

		# buy shares first
		viewwatchlist_response = ApiFacade.get_watchlist(token, watchlist_id)

		deletion_response = ApiFacade.delete_user(token)

		self.assertEqual(viewwatchlist_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}"
			.format(expected_response_code, viewwatchlist_response.get_http_status()))

		for quote in viewwatchlist_response.get_all_shares():
			for k in expected_keys_in_quote:
				self.assertIsNotNone(quote[k], msg = "Expected value for key [{0}]; got [{1}]".format(str(k), str(None)))

if __name__ == "__main__":
	unittest.main()