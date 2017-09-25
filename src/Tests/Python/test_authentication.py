#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade
from models.user_investor import User
from models.response_summary import ResponseSummary

class AuthenticationTestCase(unittest.TestCase):
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

	def test_authentication_success(self):
		"""A registered user can sign in using their correct details"""
		test_user = User("John Doe", "johndoe@test.com", "12345678")
		api = ApiFacade(test_user)
		outcome = api.authenticate_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(test_user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, True, msg = error_messages_for_display)

	def test_authentication_emailIsEmpty(self):
		"""A user cannot sign in with an empty email"""
		test_user = User("John Doe", "", "12345678")
		api = ApiFacade(test_user)
		outcome = api.authenticate_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(test_user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		correct_response = outcome.response_code != 400 and outcome.response_code != 401

		self.assertEqual(outcome.is_success or correct_response, False, msg = error_messages_for_display)

	# def test_authentication_passwordIsEmpty(self):
	# 	pass

	# def test_authentication_emailAndPasswordAreEmpty(self):
	# 	pass

	# def test_authentication_emailIsIncorrect(self):
	# 	pass

	# def test_authentication_passwordIsIncorrect(self):
	# 	pass

	# def test_authentication_emailAndPasswordAreIncorrect(self):
	# 	pass

if __name__ == "__main__":
	unittest.main()