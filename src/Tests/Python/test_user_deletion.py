#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade
from models.user_investor import User
from models.response_summary import ResponseSummary

class UserDeletionTestCase(unittest.TestCase):
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
			print("Could not delete user")

	def test_deletion_success(self):
		"""An authenticated user can delete their user account"""
		outcome = self.api.authenticate_user()
		outcome = self.api.delete_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(self.created_user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, True, msg = error_messages_for_display)

		if outcome.is_success:
			self.clean_up = False

	def test_deletion_userIsNotAuthenticated(self):
		"""An unauthenticated user cannot delete their user account"""
		outcome = self.api.delete_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(self.created_user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)


if __name__ == "__main__":
	unittest.main()