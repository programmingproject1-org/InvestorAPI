#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade
from models.response_validator import ResponseValidator

class UserAuthenticationTestCase(unittest.TestCase):

	@classmethod
	def setUpClass(cls):
		cls.api = ApiFacade()
		cls.api.register_user("John Doe", "johndoe@test.com", "12345678")

	def setUp(self):
		pass

	def tearDown(self):
		pass

	def test_authentication_success(self):
		"""A registered user can sign in using their correct details"""
		expected_response_code = 200
		input_data = ("johndoe@test.com", "12345678")
		model = {
			"accessToken": {"key_only": True, "is_collection": False},
			"expires": {"key_only": False, "is_collection": False, "value": 604800}
		}
		api = ApiFacade()
		response, token = api.authenticate_user(*input_data)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))

	def test_authentication_emailIsEmpty(self):
		"""A user cannot sign in with an empty email"""
		expected_response_code = 400
		input_data = ("", "12345678")
		expected_messages = ["The Email field is required."]
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}
		api = ApiFacade()
		response, token = api.authenticate_user(*input_data)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))

	def test_authentication_passwordIsEmpty(self):
		"""A user cannot sign in with an empty password"""
		expected_response_code = 400
		input_data = ("johndoe@test.com", "")
		expected_messages = ["The Password field is required."]
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}
		api = ApiFacade()
		response, token = api.authenticate_user(*input_data)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))

	def test_authentication_emailAndPasswordAreEmpty(self):
		"""A user cannot sign in with an empty email and an empty password"""
		expected_response_code = 400
		input_data = ("", "")
		expected_messages = ["The Email field is required.", "The Password field is required."]
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}
		api = ApiFacade()
		response, token = api.authenticate_user(*input_data)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))

	def test_authentication_emailIsIncorrect(self):
		"""A user cannot sign in with an incorrect email"""
		expected_response_code = 401
		input_data = ("wrong@test.com", "12345678")
		model = None
		api = ApiFacade()
		response, token = api.authenticate_user(*input_data)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))

	def test_authentication_passwordIsIncorrect(self):
		"""A user cannot sign in with an incorrect password"""
		expected_response_code = 401
		input_data = ("johndoe@test.com", "wwwwwwww")
		model = None
		api = ApiFacade()
		response, token = api.authenticate_user(*input_data)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))

	def test_authentication_emailAndPasswordAreIncorrect(self):
		"""A user cannot sign in with an incorrect email and an incorrect password"""
		expected_response_code = 401
		input_data = ("wrong@test.com", "wrong")
		model = None
		api = ApiFacade()
		response, token = api.authenticate_user(*input_data)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}] - {2}".format(*input_data, validator.get_errors()))

	@classmethod
	def tearDownClass(cls):
		response, token = cls.api.authenticate_user("johndoe@test.com", "12345678")
		cls.api.delete_user(token)

if __name__ == "__main__":
	unittest.main()