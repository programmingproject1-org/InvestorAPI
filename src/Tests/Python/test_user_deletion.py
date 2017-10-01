#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from api_client.api_facade import ApiFacade

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
		expected_response_code = 204
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		self.assertEqual(deletion_response.get_http_status(), expected_response_code, msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, deletion_response.get_http_status()))

	def test_deletion_userIsNotAuthenticated(self):
		"""An unauthenticated user cannot delete their user account"""
		expected_response_code = 401
		displayName, email, password = (None, None, None)
		registration_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		self.assertEqual(deletion_response.get_http_status(), expected_response_code, msg = "Expected HTTP{0}; got HTTP{1}".format(expected_response_code, deletion_response.get_http_status()))

if __name__ == "__main__":
	unittest.main()