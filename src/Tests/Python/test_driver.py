#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt import ddt, data, file_data, unpack
from io import StringIO

from models.api_facade import ApiFacade
from models.user_investor import User
from models.response_summary import ResponseSummary

@ddt
class RegistrationTestCase(unittest.TestCase):
	def setUp(self):
		self.clean_up = True

	def tearDown(self):
		if self.clean_up:
			outcome = self.api.authenticate_user()
			outcome = self.api.delete_user()
		if not outcome.is_success:
			print("Could not delete user")

	@file_data("data/registration/valid_details.json")
	def test_registration_success(self, displayName, email, password):
		"""A new user can register with valid details"""
		user = User(displayName, email, password)
		self.api = ApiFacade(user)
		outcome = self.api.register_user()
		self.assertEqual(outcome.is_success, True, msg = "\n\nTEST FAILED - HTTP: {0}: {1}".format(outcome.response_code, outcome.error_messages))
		if not outcome.is_success:
			self.clean_up = False

	# def test_registration_displayNameIsEmpty(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_emailIsEmpty(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_passwordIsEmpty(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_displayNameIsTooShort(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_passwordIsTooShort(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_displayNameIsTooLong(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_passwordIsTooLong(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_displayNameIsScript(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_emailIsScript(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_passwordIsScript(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_emailIsInvalid(self):
	# 	self.clean_up = False
	# 	pass

	# def test_registration_emailAlreadyExists(self):
	# 	self.clean_up = False
	# 	pass


if __name__ == "__main__":
	unittest.main()