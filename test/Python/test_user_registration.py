#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from api_client.api_facade import ApiFacade

@ddt
class UserRegistrationTestCase(unittest.TestCase):
	def setUp(self):
		pass

	def tearDown(self):
		pass

	def clean_up(self, email, password, api, response_code):
		""" Clean up to run after each test to delete test user if created """
		pass

	@file_data("data/registration/success.json")
	def test_registration_success(self, displayName, email, password):
		"""A new user can register with valid details"""
		expected_messages = [None]
		expected_response_code = 201

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))

	@file_data("data/registration/displayNameIsEmpty.json")
	def test_registration_displayNameIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty displayName"""
		expected_messages = ["The DisplayName field is required."]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))

	@file_data("data/registration/emailIsEmpty.json")
	def test_registration_emailIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty email"""
		expected_messages = ["The Email field is required."]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))
		
	@file_data("data/registration/passwordIsEmpty.json")
	def test_registration_passwordIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty password"""
		expected_messages = ["The Password field is required."]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))
		
	@file_data("data/registration/displayNameIsTooShort.json")
	def test_registration_displayNameIsTooShort(self, displayName, email, password):
		"""A new user cannot register with a displayName of less than 5 characters"""
		expected_messages = [
			"The field DisplayName must be a string or array type with a minimum length of '5'.",
			"The DisplayName field is required."
		]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))
		
	@file_data("data/registration/passwordIsTooShort.json")
	def test_registration_passwordIsTooShort(self, displayName, email, password):
		"""A new user cannot register with a password of less than 8 characters"""
		expected_messages = [
			"The field Password must be a string or array type with a minimum length of '8'.",
			"The Password field is required."
		]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))
		
	@file_data("data/registration/displayNameIsTooLong.json")
	def test_registration_displayNameIsTooLong(self, displayName, email, password):
		"""A new user cannot register with a displayName of more than 30 characters"""
		expected_messages = [
			"The field DisplayName must be a string or array type with a maximum length of '30'.",
			"The DisplayName field is required."
		]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))
		
	@file_data("data/registration/passwordIsTooLong.json")
	def test_registration_passwordIsTooLong(self, displayName, email, password):
		"""A new user cannot register with a password of more than 30 characters"""
		expected_messages = [
			"The field Password must be a string or array type with a maximum length of '30'.",
			"The Password field is required."
		]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))
		
	@file_data("data/registration/emailIsTooLong.json")
	def test_registration_emailIsTooLong(self, displayName, email, password):
		"""A new user cannot register with an email of more than 100 characters"""
		expected_messages = [
			"The field Email must be a string or array type with a maximum length of '100'.",
			"The email address is invalid."
		]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))

	@file_data("data/registration/emailIsInvalid.json")
	def test_registration_emailIsInvalid(self, displayName, email, password):
		"""A new user cannot register with an invalid email"""
		expected_messages = ["The email address is invalid."]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))

	@file_data("data/registration/displayNameIsNotAlphaNumeric.json")
	def test_registration_displayNameIsNotAlphaNumeric(self, displayName, email, password):
		"""A new user cannot register with a non-alphanumeric displayName"""
		expected_messages = ["""The field DisplayName must match the regular expression '^[^~`^$#@%!'*\\(\\)<>=.;:]+$'."""]
		expected_response_code = 400

		response_wrapper = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, response_wrapper.get_http_status(),
				displayName, email, password))
		self.assertEqual(response_body_match, True, 
			msg = "Expected [{0}]; got [{1}]".format(expected_messages, response_wrapper.get_message()))

if __name__ == "__main__":
	unittest.main()