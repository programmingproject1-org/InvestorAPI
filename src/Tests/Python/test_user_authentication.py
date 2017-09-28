#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from pprint import pprint
from third_party.ddt.ddt import ddt, data, file_data, unpack

from models.api_facade import ApiFacade

class UserAuthenticationTestCase(unittest.TestCase):

	@classmethod
	def setUpClass(cls):
		cls.api = ApiFacade()
		cls.api.register_user("John Doe", "johndoe@test.com", "12345678")

	def setUp(self):
		pass

	def tearDown(self):
		pass

	def test_authentication_success(self):
		"""A registered user can sign in using their correct details"""
		api = ApiFacade()
		code, message = api.authenticate_user("johndoe@test.com", "12345678")
		self.assertEqual(code, 200)

	def test_authentication_emailIsEmpty(self):
		"""A user cannot sign in with an empty email"""
		api = ApiFacade()
		code, message = api.authenticate_user("", "12345678")
		self.assertEqual(code, 400)

	def test_authentication_passwordIsEmpty(self):
		"""A user cannot sign in with an empty password"""
		api = ApiFacade()
		code, message = api.authenticate_user("johndoe@test.com", "")
		self.assertEqual(code, 400)

	def test_authentication_emailAndPasswordAreEmpty(self):
		"""A user cannot sign in with an empty email and an empty password"""
		api = ApiFacade()
		code, message = api.authenticate_user("", "")
		self.assertEqual(code, 400)

	def test_authentication_emailIsIncorrect(self):
		"""A user cannot sign in with an incorrect email"""
		api = ApiFacade()
		code, message = api.authenticate_user("wrong@test.com", "12345678")
		self.assertEqual(code, 401)

	def test_authentication_passwordIsIncorrect(self):
		"""A user cannot sign in with an incorrect password"""
		api = ApiFacade()
		code, message = api.authenticate_user("johndoe@test.com", "wwwwwwww")
		self.assertEqual(code, 401)

	def test_authentication_emailAndPasswordAreIncorrect(self):
		"""A user cannot sign in with an incorrect email and an incorrect password"""
		api = ApiFacade()
		code, message = api.authenticate_user("wrong@test.com", "wrong")
		self.assertEqual(code, 401)

	@classmethod
	def tearDownClass(cls):
		code, token = cls.api.authenticate_user("johndoe@test.com", "12345678")
		cls.api.delete_user(token)

if __name__ == "__main__":
	unittest.main()