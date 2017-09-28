#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade

@ddt
class UserRegistrationTestCase(unittest.TestCase):
	def setUp(self):
		pass

	def tearDown(self):
		pass

	def clean_up(self, email, password, api, response_code):
		if response_code != 201: return
		auth_status_code, token = api.authenticate_user(email, password)
		if token is not None and api.delete_user(token) != 204:
			print("Could not delete test user [{0}, {1}]".format(email, password))

	def validate_response(self, response_code, response_message, expected_response_code, expected_messages = None):
		is_pass = ((response_code == expected_response_code) and (expected_messages is None or response_message in expected_messages))
		return is_pass

	def stringify_result(self, displayName, email, password, server_response_code, server_message, expected_status, expected_messages):
		if expected_messages is None:
			expected_messages = ['None']

		result = "For data: [displayName: '{0}', email: '{1}', password: '{2}'];".format(displayName, email, password)
		result += "Got [HTTP {0} - {1}];".format(server_response_code, server_message)
		result += "Expected: [HTTP {0} - {1}]".format(expected_status, " or ".join(expected_messages))
		return result

	def run_test(self, displayName, email, password, expected_messages, expected_status):
		api = ApiFacade()
		response_code, response_message = api.register_user(displayName, email, password)
		outcome = self.validate_response(response_code, response_message, expected_status, expected_messages)
		test_output = self.stringify_result(displayName, email, password, response_code, response_message, expected_status, expected_messages)
		self.clean_up(email, password, api, expected_status) 
		return (outcome, test_output)

	@file_data("data/registration/success.json")
	def test_registration_success(self, displayName, email, password):
		"""A new user can register with valid details"""
		expected_messages = None
		expected_status = 201
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)

	@file_data("data/registration/displayNameIsEmpty.json")
	def test_registration_displayNameIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty displayName"""
		expected_messages = ["The DisplayName field is required."]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)

	@file_data("data/registration/emailIsEmpty.json")
	def test_registration_emailIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty email"""
		expected_messages = ["The Email field is required."]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	@file_data("data/registration/passwordIsEmpty.json")
	def test_registration_passwordIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty password"""
		expected_messages = ["The Password field is required."]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	@file_data("data/registration/displayNameIsTooShort.json")
	def test_registration_displayNameIsTooShort(self, displayName, email, password):
		"""A new user cannot register with a displayName of less than 5 characters"""
		expected_messages = [
			"The field DisplayName must be a string or array type with a minimum length of '5'.",
			"The DisplayName field is required."
		]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	@file_data("data/registration/passwordIsTooShort.json")
	def test_registration_passwordIsTooShort(self, displayName, email, password):
		"""A new user cannot register with a password of less than 8 characters"""
		expected_messages = [
			"The field Password must be a string or array type with a minimum length of '8'.",
			"The Password field is required."
		]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	@file_data("data/registration/displayNameIsTooLong.json")
	def test_registration_displayNameIsTooLong(self, displayName, email, password):
		"""A new user cannot register with a displayName of more than 30 characters"""
		expected_messages = [
			"The field DisplayName must be a string or array type with a maximum length of '30'.",
			"The DisplayName field is required."
		]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	@file_data("data/registration/passwordIsTooLong.json")
	def test_registration_passwordIsTooLong(self, displayName, email, password):
		"""A new user cannot register with a password of more than 30 characters"""
		expected_messages = [
			"The field Password must be a string or array type with a maximum length of '30'.",
			"The Password field is required."
		]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	@file_data("data/registration/emailIsTooLong.json")
	def test_registration_emailIsTooLong(self, displayName, email, password):
		"""A new user cannot register with an email of more than 100 characters"""
		expected_messages = [
			"The field Email must be a string or array type with a maximum length of '100'.",
			"The email address is invalid."
		]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	# @file_data("data/registration/displayNameIsScript.json")
	# def test_registration_displayNameIsScript(self, displayName, email, password):
	# 	"""A new user cannot register with HTML or JS code in their displayName"""
	# 	user = User(displayName, email, password)
	# 	self.api = ApiFacade(user)
	# 	outcome = self.api.register_user()

	# 	error_messages = []
	# 	error_messages.append({"Failure on Test data": str(user)})
	# 	error_messages.append(outcome.error_messages)
	# 	error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

	# 	self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
	# 	if not outcome.is_success:
	# 		self.clean_up = False

	@file_data("data/registration/emailIsInvalid.json")
	def test_registration_emailIsInvalid(self, displayName, email, password):
		"""A new user cannot register with an invalid email"""
		expected_messages = ["The email address is invalid."]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	# @file_data("data/registration/emailAlreadyExists.json")
	# def test_registration_emailAlreadyExists(self, displayName, email, password):
	# 	"""A new user cannot register with an email associated with an existing user"""
	# 	user = User(displayName, email, password)
	# 	self.api = ApiFacade(user)
	# 	first_outcome = self.api.register_user()
	# 	second_outcome = self.api.register_user()

	# 	error_messages = []
	# 	error_messages.append({"Failure on Test data": str(user)})
	# 	error_messages.append(second_outcome.error_messages)
	# 	error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(second_outcome.response_code, error_messages)

	# 	self.assertEqual(second_outcome.is_success, False, msg = error_messages_for_display)
	# 	if not first_outcome.is_success and not second_outcome.is_success:
	# 		self.clean_up = False

	@file_data("data/registration/displayNameIsNotAlphaNumeric.json")
	def test_registration_displayNameIsNotAlphaNumeric(self, displayName, email, password):
		"""A new user cannot register with a non-alphanumeric displayName"""
		expected_messages = ["""The field DisplayName must match the regular expression '^[^~`^$#@%!'*\\(\\)<>=.;:]+$'."""]
		expected_status = 400
		outcome, test_output = self.run_test(displayName, email, password, expected_messages, expected_status)
		self.assertEqual(outcome, True, msg = test_output)
		
	# @file_data("data/registration/emailIsNotAlphaNumeric.json")
	# def test_registration_emailIsNotAlphaNumeric(self, displayName, email, password):
	# 	"""A new user cannot register with a non-alphanumeric email"""
	# 	user = User(displayName, email, password)
	# 	self.api = ApiFacade(user)
	# 	first_outcome = self.api.register_user()
	# 	second_outcome = self.api.register_user()

	# 	error_messages = []
	# 	error_messages.append({"Failure on Test data": str(user)})
	# 	error_messages.append(second_outcome.error_messages)
	# 	error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(second_outcome.response_code, error_messages)

	# 	self.assertEqual(second_outcome.is_success, False, msg = error_messages_for_display)
	# 	if not first_outcome.is_success and not second_outcome.is_success:
	# 		self.clean_up = False

if __name__ == "__main__":
	unittest.main()