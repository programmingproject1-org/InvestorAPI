#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade
from models.response_validator import ResponseValidator

@ddt
class UserRegistrationTestCase(unittest.TestCase):
	def setUp(self):
		pass

	def tearDown(self):
		pass

	def clean_up(self, email, password, api, response_code):
		""" Clean up to run after each test to delete test user if created """
		auth_status_code, token = api.authenticate_user(email, password)
		if token is not None and api.delete_user(token) != 204:
			print("Could not delete test user [{0}, {1}]".format(email, password))

	@file_data("data/registration/success.json")
	def test_registration_success(self, displayName, email, password):
		"""A new user can register with valid details"""
		expected_messages = None
		expected_response_code = 201
		input_data = (displayName, email, password)
		model = None
		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))

	@file_data("data/registration/displayNameIsEmpty.json")
	def test_registration_displayNameIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty displayName"""
		expected_messages = ["The DisplayName field is required."]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))

	@file_data("data/registration/emailIsEmpty.json")
	def test_registration_emailIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty email"""
		expected_messages = ["The Email field is required."]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		
	@file_data("data/registration/passwordIsEmpty.json")
	def test_registration_passwordIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty password"""
		expected_messages = ["The Password field is required."]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		
	@file_data("data/registration/displayNameIsTooShort.json")
	def test_registration_displayNameIsTooShort(self, displayName, email, password):
		"""A new user cannot register with a displayName of less than 5 characters"""
		expected_messages = [
			"The field DisplayName must be a string or array type with a minimum length of '5'.",
			"The DisplayName field is required."
		]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		
	@file_data("data/registration/passwordIsTooShort.json")
	def test_registration_passwordIsTooShort(self, displayName, email, password):
		"""A new user cannot register with a password of less than 8 characters"""
		expected_messages = [
			"The field Password must be a string or array type with a minimum length of '8'.",
			"The Password field is required."
		]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		
	@file_data("data/registration/displayNameIsTooLong.json")
	def test_registration_displayNameIsTooLong(self, displayName, email, password):
		"""A new user cannot register with a displayName of more than 30 characters"""
		expected_messages = [
			"The field DisplayName must be a string or array type with a maximum length of '30'.",
			"The DisplayName field is required."
		]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		
	@file_data("data/registration/passwordIsTooLong.json")
	def test_registration_passwordIsTooLong(self, displayName, email, password):
		"""A new user cannot register with a password of more than 30 characters"""
		expected_messages = [
			"The field Password must be a string or array type with a maximum length of '30'.",
			"The Password field is required."
		]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		
	@file_data("data/registration/emailIsTooLong.json")
	def test_registration_emailIsTooLong(self, displayName, email, password):
		"""A new user cannot register with an email of more than 100 characters"""
		expected_messages = [
			"The field Email must be a string or array type with a maximum length of '100'.",
			"The email address is invalid."
		]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))

	@file_data("data/registration/emailIsInvalid.json")
	def test_registration_emailIsInvalid(self, displayName, email, password):
		"""A new user cannot register with an invalid email"""
		expected_messages = ["The email address is invalid."]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))

	@file_data("data/registration/displayNameIsNotAlphaNumeric.json")
	def test_registration_displayNameIsNotAlphaNumeric(self, displayName, email, password):
		"""A new user cannot register with a non-alphanumeric displayName"""
		expected_messages = ["""The field DisplayName must match the regular expression '^[^~`^$#@%!'*\\(\\)<>=.;:]+$'."""]
		expected_response_code = 400
		input_data = (displayName, email, password)
		model = {
			"message": {
				"key_only": True, 
				"is_collection": False, 
				"value": expected_messages
			}
		}

		api = ApiFacade()
		response = api.register_user(displayName, email, password)
		validator = ResponseValidator(response, expected_response_code, model)
		correct_status, status = validator.response_code_success()
		correct_body = validator.response_body_success()

		if status == 201: self.clean_up(email, password, api, status)
		self.assertEqual(correct_status, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))
		self.assertEqual(correct_body, True, msg = "On data [{0}][{1}][{2}] - {3}".format(*input_data, validator.get_errors()))

if __name__ == "__main__":
	unittest.main()