#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

class ViewTransactionsTestCase(unittest.TestCase):
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

	def test_view_transactions_success(self):
		"""An authenticated user can sell shares that they own"""
		expected_response_code = 200
		expected_keys = ["items", "pageNumber", "pageSize", "totalPageCount", "totalRowCount"]
		symbol = "DDD" 
		quantity = 100
		
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		token = authentication_response.get_token()

		# get account id
		viewdetails_response = ApiFacade.view_details(token)
		account_id = viewdetails_response.get_main_account_id()

		# buy a share
		buyshare_response = ApiFacade.buy_share(token, account_id, symbol, quantity)

		# buy shares first
		transactions_response = ApiFacade.get_transactions(token, account_id, page_number = None, 
			page_size = None, start_date = None, end_date = None)

		deletion_response = ApiFacade.delete_user(token)

		self.assertEqual(transactions_response.get_http_status(), expected_response_code, 
			msg = "Expected HTTP{0}; got HTTP{1}"
			.format(expected_response_code, transactions_response.get_http_status()))

		for k in expected_keys:
			self.assertEqual(k in transactions_response.get_json_body(), True, msg = "Expected key [{0}]; got [{1}]".format(k, str(None)))

		#pprint(transactions_response.get_items())

		self.assertEqual(len(transactions_response.get_items()), 4, msg = "Expected {0} item in response; got {1} instead".format(1, len(transactions_response.get_items())))

if __name__ == "__main__":
	unittest.main()