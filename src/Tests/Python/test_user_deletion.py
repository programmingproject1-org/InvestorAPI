#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade
from models.response_validator import ResponseValidator

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
		expected_messages = None
		expected_response_code = 204
		input_data = ("John Doe", "johndoe@test.com", "12345678")
		model = None
		api = ApiFacade()
		response = api.register_user(*input_data)
		response, token = api.authenticate_user(*input_data[1:3])
		response = api.delete_user(token)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))

	def test_deletion_userIsNotAuthenticated(self):
		"""An unauthenticated user cannot delete their user account"""
		expected_messages = None
		expected_response_code = 401
		input_data = (None, None, None)
		model = None
		api = ApiFacade()
		response = api.delete_user(token = None)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))

if __name__ == "__main__":
	unittest.main()