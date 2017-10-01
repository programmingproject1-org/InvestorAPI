#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json

class ViewDetailsResponseWrapper():

	SUCCESS_STATUS = 200

	def __init__(self, response):
		self.response = response

	def get_http_status(self):
		return self.response.status_code

	def get_json_body(self):
		try:
			body = self.response.json()
		except ValueError:
			body = None
		return body

	def get_id(self):
		try:
			body = self.get_json_body()
			user_id = body["id"]
		except:
			user_id = None
		return user_id

	def get_email(self):
		try:
			body = self.get_json_body()
			user_email = body["email"]
		except:
			user_email = None
		return user_email

	def get_displayName(self):
		try:
			body = self.get_json_body()
			user_displayName = body["displayName"]
		except:
			user_displayName = None
		return user_displayName

	def get_level(self):
		try:
			body = self.get_json_body()
			user_level = body["level"]
		except:
			user_level = None
		return user_level

	def get_accounts(self):
		try:
			body = self.get_json_body()
			user_accounts = body["accounts"]
		except:
			user_accounts = None
		return user_accounts

	def get_watchlists(self):
		try:
			body = self.get_json_body()
			user_watchlists = body["watchlists"]
		except:
			user_watchlists = None
		return user_watchlists
