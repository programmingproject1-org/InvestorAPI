#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade
from models.user_investor import User
from models.response_summary import ResponseSummary

@ddt
class UserRegistrationTestCase(unittest.TestCase):
	def setUp(self):
		self.clean_up = True

	def tearDown(self):
		tearDown_success = False
		if self.clean_up:
			outcome = self.api.authenticate_user()
			outcome = self.api.delete_user()
			tearDown_success = outcome.is_success
		if self.clean_up and not tearDown_success:
			print("Could not delete user")

	@file_data("data/registration/success.json")
	def test_registration_success(self, displayName, email, password):
		"""A new user can register with valid details"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, True, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/displayNameIsEmpty.json")
	def test_registration_displayNameIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty displayName"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)

		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/emailIsEmpty.json")
	def test_registration_emailIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty email"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/passwordIsEmpty.json")
	def test_registration_passwordIsEmpty(self, displayName, email, password):
		"""A new user cannot register with an empty password"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)		
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/displayNameIsTooShort.json")
	def test_registration_displayNameIsTooShort(self, displayName, email, password):
		"""A new user cannot register with a displayName of less than 5 characters"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/passwordIsTooShort.json")
	def test_registration_passwordIsTooShort(self, displayName, email, password):
		"""A new user cannot register with a password of less than 8 characters"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/displayNameIsTooLong.json")
	def test_registration_displayNameIsTooLong(self, displayName, email, password):
		"""A new user cannot register with a displayName of more than 30 characters"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/passwordIsTooLong.json")
	def test_registration_passwordIsTooLong(self, displayName, email, password):
		"""A new user cannot register with a password of more than 30 characters"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/emailIsTooLong.json")
	def test_registration_emailIsTooLong(self, displayName, email, password):
		"""A new user cannot register with an email of more than 100 characters"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/displayNameIsScript.json")
	def test_registration_displayNameIsScript(self, displayName, email, password):
		"""A new user cannot register with HTML or JS code in their displayName"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/emailIsScript.json")
	def test_registration_emailIsScript(self, displayName, email, password):
		"""A new user cannot register with HTML or JS code in their email"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	# @file_data("data/registration/passwordIsScript.json")
	# def test_registration_passwordIsScript(self, displayName, email, password):
	# 	"""A new user cannot register with HTML or JS code in their password"""
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
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(outcome.response_code, error_messages)

		self.assertEqual(outcome.is_success, False, msg = error_messages_for_display)
		if not outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/emailAlreadyExists.json")
	def test_registration_emailAlreadyExists(self, displayName, email, password):
		"""A new user cannot register with an email associated with an existing user"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		first_outcome = self.api.register_user()
		second_outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(second_outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(second_outcome.response_code, error_messages)

		self.assertEqual(second_outcome.is_success, False, msg = error_messages_for_display)
		if not first_outcome.is_success and not second_outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/displayNameIsNotAlphaNumeric.json")
	def test_registration_displayNameIsNotAlphaNumeric(self, displayName, email, password):
		"""A new user cannot register with a non-alphanumeric displayName"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		first_outcome = self.api.register_user()
		second_outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(second_outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(second_outcome.response_code, error_messages)

		self.assertEqual(second_outcome.is_success, False, msg = error_messages_for_display)
		if not first_outcome.is_success and not second_outcome.is_success:
			self.clean_up = False

	@file_data("data/registration/emailIsNotAlphaNumeric.json")
	def test_registration_emailIsNotAlphaNumeric(self, displayName, email, password):
		"""A new user cannot register with a non-alphanumeric email"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		first_outcome = self.api.register_user()
		second_outcome = self.api.register_user()

		error_messages = []
		error_messages.append({"Failure on Test data": str(user)})
		error_messages.append(second_outcome.error_messages)
		error_messages_for_display = "\n\nTEST FAILED - Received HTTP: {0}: {1}".format(second_outcome.response_code, error_messages)

		self.assertEqual(second_outcome.is_success, False, msg = error_messages_for_display)
		if not first_outcome.is_success and not second_outcome.is_success:
			self.clean_up = False

if __name__ == "__main__":
	unittest.main()