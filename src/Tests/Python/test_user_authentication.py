#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from api_client.api_facade import ApiFacade

class UserAuthenticationTestCase(unittest.TestCase):

	TEST_USER = {
		"displayName": "John Doe",
		"email": "johndoe@test.com",
		"password": "12345678"
	}

	@classmethod
	def setUpClass(self):
		ApiFacade.register_user(self.TEST_USER["displayName"], self.TEST_USER["email"], self.TEST_USER["password"])

	def setUp(self):
		pass

	def tearDown(self):
		pass

	def test_authentication_success(self):
		"""A registered user can sign in using their correct details"""
		expected_messages = [None]
		expected_response_code = 200
		response_wrapper = ApiFacade.authenticate_user(self.TEST_USER["email"], self.TEST_USER["password"])
		
		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, msg = "HTTP must be {0}".format(expected_response_code))
		self.assertEqual(response_body_match, True, msg = "Did not expect a Message in response")

	def test_authentication_emailIsEmpty(self):
		"""A user cannot sign in with an empty email"""
		expected_response_code = 400
		expected_messages = ["The Email field is required."]
		response_wrapper = ApiFacade.authenticate_user("", "12345678")

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, msg = "HTTP must be {0}".format(expected_response_code))
		self.assertEqual(response_body_match, True, msg = "Did not expect a Message in response")

	def test_authentication_passwordIsEmpty(self):
		"""A user cannot sign in with an empty password"""
		expected_response_code = 400
		expected_messages = ["The Password field is required."]
		response_wrapper = ApiFacade.authenticate_user("johndoe@test.com", "")

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, msg = "HTTP must be {0}".format(expected_response_code))
		self.assertEqual(response_body_match, True, msg = "Did not expect a Message in response")

	def test_authentication_emailAndPasswordAreEmpty(self):
		"""A user cannot sign in with an empty email and an empty password"""
		expected_response_code = 400
		expected_messages = ["The Email field is required.", "The Password field is required."]
		response_wrapper = ApiFacade.authenticate_user("", "")

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, msg = "HTTP must be {0}".format(expected_response_code))
		self.assertEqual(response_body_match, True, msg = "Did not expect a Message in response")

	def test_authentication_emailIsIncorrect(self):
		"""A user cannot sign in with an incorrect email"""
		expected_response_code = 401
		expected_messages = [None]
		response_wrapper = ApiFacade.authenticate_user("wrong@test.com", "12345678")

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, msg = "HTTP must be {0}".format(expected_response_code))
		self.assertEqual(response_body_match, True, msg = "Did not expect a Message in response")

	def test_authentication_passwordIsIncorrect(self):
		"""A user cannot sign in with an incorrect password"""
		expected_response_code = 401
		expected_messages = [None]
		response_wrapper = ApiFacade.authenticate_user("johndoe@test.com", "wwwwwwww")

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, msg = "HTTP must be {0}".format(expected_response_code))
		self.assertEqual(response_body_match, True, msg = "Did not expect a Message in response")

	def test_authentication_emailAndPasswordAreIncorrect(self):
		"""A user cannot sign in with an incorrect email and an incorrect password"""
		expected_response_code = 401
		expected_messages = [None]
		response_wrapper = ApiFacade.authenticate_user("wrong@test.com", "wrong")

		response_status_match = response_wrapper.get_http_status() == expected_response_code
		response_body_match = response_wrapper.get_message() in expected_messages

		self.assertEqual(response_status_match, True, msg = "HTTP must be {0}".format(expected_response_code))
		self.assertEqual(response_body_match, True, msg = "Did not expect a Message in response")

	@classmethod
	def tearDownClass(cls):
		pass

if __name__ == "__main__":
	unittest.main()