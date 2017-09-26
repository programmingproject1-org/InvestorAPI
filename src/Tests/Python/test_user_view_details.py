#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade
from models.user_investor import User
from models.response_summary import ResponseSummary

class UserViewDetailsTestCase(unittest.TestCase):
	def setUp(self):
		self.clean_up = True
		self.created_user = User("John Doe", "johndoe@test.com", "12345678")
		self.api = ApiFacade(self.created_user)
		outcome = self.api.register_user()

	def tearDown(self):
		tearDown_success = False
		if self.clean_up:
			outcome = self.api.authenticate_user()
			outcome = self.api.delete_user()
			tearDown_success = outcome.is_success
		if self.clean_up and not tearDown_success:
			print("Could not delete user: {0}{1}{2}".format(str(self.created_user), outcome.error_messages, outcome.response_code))

	def test_viewDetails_success(self):
		"""An authenticated user can view their user account details"""
		api = ApiFacade(self.created_user)
		outcome = api.authenticate_user()
		outcome = api.view_details()

		error_messages = []
		error_messages.append({"Failure on Test data": str(self.created_user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, True, msg = error_messages_for_display)

	def test_viewDetails_unauthenticated(self):
		"""An unauthenticated user cannot view their user account details"""
		api = ApiFacade(self.created_user)
		outcome = api.view_details()

		error_messages = []
		error_messages.append({"Failure on Test data": str(self.created_user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		correct_response = outcome.response_code != 400 and outcome.response_code != 401

		self.assertEqual(outcome.is_success or correct_response, False, msg = error_messages_for_display)

if __name__ == "__main__":
	unittest.main()