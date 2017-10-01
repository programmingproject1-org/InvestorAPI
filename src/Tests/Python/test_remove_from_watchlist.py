#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")

@ddt
class RemoveFromWatchlistTestCase(unittest.TestCase):
	@classmethod
	def setUpClass(cls):
		global displayName, email, password
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		cls.token = authentication_response.get_token()

	@classmethod
	def tearDownClass(cls):
		# delete test user
		deletion_response = ApiFacade.delete_user(cls.token)

	def setUp(self):
		pass

	def tearDown(self):
		pass

	@file_data("data/current_quotes/single_symbol_success.json")
	def test_remove_from_watchlist_success(self, symbol):
		"""A user can remove a share to their watchlist"""
		global displayName, email, password

		expected_response_code = 204
		symbol = symbol
		
		authentication_response = ApiFacade.authenticate_user(email, password)
		token = authentication_response.get_token()

		# get watchlist id
		viewdetails_response = ApiFacade.view_details(token)
		watchlist_id = viewdetails_response.get_main_watchlist_id()

		# add symbol to watchlist
		addtowatchlist_response = ApiFacade.add_to_watchlist(token, watchlist_id, symbol)

		# get updated watchlist
		viewwatchlist_response = ApiFacade.get_watchlist(token, watchlist_id)

		print("ADDED:")
		pprint(viewwatchlist_response.get_json_body())

		# remove symbol from watchlist
		removefromwatchlist_response = ApiFacade.remove_from_watchlist(token, watchlist_id, symbol)

		# get updated watchlist
		viewwatchlist_response = ApiFacade.get_watchlist(token, watchlist_id)
		
		print("REMOVED:")
		pprint(viewwatchlist_response.get_json_body())

		# check request went through
		self.assertEqual(removefromwatchlist_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}"
			.format(expected_response_code, removefromwatchlist_response.get_http_status()))

		# check watchlist has been updated
		self.assertIsNone(removefromwatchlist_response.get_share_by_symbol(symbol), 
			msg = "Expected updated watchlist to contain {0} symbol {1}; got {2} match"
			.format(1, symbol, str(None)))

if __name__ == "__main__":
	unittest.main()