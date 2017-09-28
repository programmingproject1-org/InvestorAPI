#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade

class UserDeletionTestCase(unittest.TestCase):

	@classmethod
	def setUpClass(cls):
		pass

	def setUp(self):
		pass

	def tearDown(self):
		pass

	def test_deletion_success(self):
		"""An authenticated user can delete their user account"""
		api = ApiFacade()
		response_code = api.register_user("John Doe", "johndoe@test.com", "12345678")
		response_code, token = api.authenticate_user("johndoe@test.com", "12345678")
		response_code = api.delete_user(token)
		self.assertEqual(response_code, 204, msg = "fail")

	def test_deletion_userIsNotAuthenticated(self):
		"""An unauthenticated user cannot delete their user account"""
		api = ApiFacade()
		response_code = api.delete_user(token = None)
		self.assertEqual(response_code, 401, msg = "fail")

if __name__ == "__main__":
	unittest.main()