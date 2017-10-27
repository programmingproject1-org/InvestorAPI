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

	def clean_up(self):
		""" Clean up to run after each test to delete test user if created """
		pass

	def test_edit_user_success(self):
		"""A user can edit their profile with valid details"""
		expected_messages = [None]
		expected_response_code = 204
		displayName, email, password = ("John Doe", "johndoe@test.com", "12345678")
		new_displayName, new_email = ("Edited John", "editedjohn@test.com")

		register_response = ApiFacade.register_user(displayName, email, password)
		authentication_response = ApiFacade.authenticate_user(email, password)
		token = authentication_response.get_token()
		edituser_response = ApiFacade.edit_user(token, new_displayName, new_email)
		viewdetails_response = ApiFacade.view_details(token)
		#deletion_response = ApiFacade.delete_user(authentication_response.get_token())

		response_status_match = edituser_response.get_http_status() == expected_response_code

		self.assertEqual(response_status_match, True, 
			msg = "Expected HTTP{0}; got HTTP{1}; on data [{2}][{3}][{4}]"
			.format(expected_response_code, edituser_response.get_http_status(),
				displayName, email, password))

		self.assertEqual(viewdetails_response.get_email(), new_email,
			msg = "Email not updated; expected {{0}}; got {{1}}"
			.format(new_email, viewdetails_response.get_email()))

		self.assertEqual(viewdetails_response.get_displayName(), new_displayName,
			msg = "Email not updated; expected {{0}}; got {{1}}"
			.format(new_displayName, viewdetails_response.get_displayName()))

if __name__ == "__main__":
	unittest.main()