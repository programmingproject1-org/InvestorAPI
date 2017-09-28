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
		user_params = ("John Doe", "johndoe@test.com", "12345678")
		response_code = api.register_user(*user_params)
		response_code, token = api.authenticate_user(*user_params[1:3])
		response_code = api.delete_user(token)
		self.assertEqual(response_code, 204, msg = "For data: [{0}]; Got: [HTTP {1}]; Expected: [HTTP {2}]"
			.format(';'.join(user_params), response_code, 204))

	def test_deletion_userIsNotAuthenticated(self):
		"""An unauthenticated user cannot delete their user account"""
		api = ApiFacade()
		response_code = api.delete_user(token = None)
		self.assertEqual(response_code, 401, msg = "Got: [HTTP {0}]; Expected: [HTTP {1}]".format(response_code, 401))

if __name__ == "__main__":
	unittest.main()